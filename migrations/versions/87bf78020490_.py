"""empty message

Revision ID: 87bf78020490
Revises: ddb17ca28608
Create Date: 2024-04-27 00:36:26.765951

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '87bf78020490'
down_revision = 'ddb17ca28608'
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('stocks', schema=None) as batch_op:
        batch_op.drop_column('close')
        batch_op.drop_column('shares_outstanding')
        batch_op.drop_column('low')
        batch_op.drop_column('institutional_info')
        batch_op.drop_column('open')
        batch_op.drop_column('volume')
        batch_op.drop_column('high')
        batch_op.drop_column('last_price')

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('stocks', schema=None) as batch_op:
        batch_op.add_column(sa.Column('last_price', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('high', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('volume', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('open', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('institutional_info', sa.TEXT(), nullable=True))
        batch_op.add_column(sa.Column('low', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('shares_outstanding', sa.FLOAT(), nullable=True))
        batch_op.add_column(sa.Column('close', sa.FLOAT(), nullable=True))

    # ### end Alembic commands ###

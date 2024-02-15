"""empty message

Revision ID: 53946a4c6705
Revises: 9ddd501513c7
Create Date: 2024-02-14 10:53:32.090065

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '53946a4c6705'
down_revision = '9ddd501513c7'
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('stocks', schema=None) as batch_op:
        batch_op.add_column(sa.Column('number_of_tutes', sa.String(length=10), nullable=True))
        batch_op.add_column(sa.Column('percentage_tutes', sa.String(length=10), nullable=True))
        batch_op.add_column(sa.Column('percentage_insiders', sa.String(length=10), nullable=True))

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('stocks', schema=None) as batch_op:
        batch_op.drop_column('percentage_insiders')
        batch_op.drop_column('percentage_tutes')
        batch_op.drop_column('number_of_tutes')

    # ### end Alembic commands ###
